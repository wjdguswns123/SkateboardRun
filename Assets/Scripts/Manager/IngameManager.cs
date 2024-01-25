using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private PlayerController _player;

    #endregion

    private Enums.eGameState _gameState;
    public Enums.eGameState GameState { get { return _gameState; } }

    public PlayerController Player { get { return _player; } }

    private StageController _currentStageCtrl;

    private IngameUI _ingameUI;

    private int _clearScore;
    private int _currentGetCoinCount;
    private int _skillScore;
    private int _coinScore;
    private int _currentScore;

    private Stage _currentStageData;

    private BackgroundController _bgCtrl => _currentStageCtrl.BGController;

    private void Start()
    {
        _gameState = Enums.eGameState.Loading;
        _coinScore = ConfigData.Instance.GetData("COIN_SCORE").IntValue;

        StartCoroutine(OnStartGame());
    }

    /// <summary>
    /// ���� ���� ó��.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnStartGame()
    {
        SystemUIManager.Instance.ShowLoadingUI();

        PlayerModel.Instance.Init();
        StageModel.Instance.Init();

        _currentStageData = StageData.Instance.GetData(StageModel.Instance.CurrentStageIndex);

        yield return null;

        LoadStage();

        yield return EffectManager.Instance.PreloadEffect();    // ����Ʈ �����ε�.

        LoadUI();

        _clearScore = _currentStageData.Star1Score;

        yield return YieldCache.WaitForSeconds(1f); // �ε� UI ������ �ӽ÷� 1�� ������.

        SystemUIManager.Instance.HideLoadingUI();

        StartGame();
    }

    /// <summary>
    /// �������� ������ �ҷ�����.
    /// </summary>
    private void LoadStage()
    {
        string stagePath = ConstantValues.PATH_STAGE_PREFAB + _currentStageData.ResourcePath;
        var stageObject = Instantiate(Resources.Load(stagePath)) as GameObject;
        _currentStageCtrl = stageObject.GetComponent<StageController>();
    }

    /// <summary>
    /// UI �ҷ�����.
    /// </summary>
    private void LoadUI()
    {
        _ingameUI = UIManager.Instance.LoadUI("IngameUI").GetComponent<IngameUI>();
    }

    private void LateUpdate()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            var currentPlayerPosX = _player.transform.position.x;
            // ���� ���� ���� ǥ��.
            var currentRate = _currentStageCtrl.GetCurrentRate(currentPlayerPosX);
            _currentScore = (int)(_clearScore * currentRate) + _currentGetCoinCount * _coinScore + _skillScore;

            _ingameUI.SetScoreUI(_currentScore);

            _bgCtrl.UpdateBackground(_currentStageCtrl.GetMoveLength(currentPlayerPosX));

            if(currentRate >= 1f)
            {
                EndGame(true);
            }
        }
    }

    #region Process

    /// <summary>
    /// ���� ����.
    /// </summary>
    public void StartGame()
    {
        _currentStageCtrl.Init(_player.transform);
        _player.Init();
        _bgCtrl.Init(_player.transform.localPosition);
        _ingameUI.Init();
        _currentGetCoinCount = 0;
        _skillScore = 0;
        _gameState = Enums.eGameState.Playing;
    }

    /// <summary>
    /// ���� �����.
    /// </summary>
    public void RestartGame()
    {
        SetGamePause(false);
        StartGame();
    }

    /// <summary>
    /// ���� �Ͻ����� ó��.
    /// </summary>
    public void SetGamePause(bool isPause)
    {
        if (isPause)
        {
            _gameState = Enums.eGameState.Pause;
            Time.timeScale = 0f;
        }
        else
        {
            _gameState = Enums.eGameState.Playing;
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// ���� ����.
    /// </summary>
    public void EndGame(bool isSuccess)
    {
        _gameState = Enums.eGameState.EndGame;

        if(isSuccess)
        {
            _player.Stop();
        }
        else
        {
            _player.Die();
        }

        StartCoroutine(DelayShowResultUI(isSuccess));
    }

    /// <summary>
    /// ���� �ð� ���� ���� ��� UI ���.
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    private IEnumerator DelayShowResultUI(bool isSuccess)
    {
        yield return YieldCache.WaitForSeconds(1f);

        int star = 1;
        if(isSuccess)
        {
            if(_currentScore >= _currentStageData.Star3Score)
            {
                star = 3;
            }
            else if (_currentScore >= _currentStageData.Star2Score)
            {
                star = 2;
            }
        }

        GameResultUI resultUI = UIManager.Instance.LoadUI("GameResultUI").GetComponent<GameResultUI>();
        resultUI.ShowResult(isSuccess, star);
    }

    /// <summary>
    /// ���� ������.
    /// </summary>
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    #endregion

    /// <summary>
    /// ���� ȹ�� ó��.
    /// </summary>
    public void GetCoin()
    {
        ++_currentGetCoinCount;
    }

    /// <summary>
    /// ĳ���� ���� ����.
    /// </summary>
    public void PlayJump(int skillIndex)
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            //// ��ų ���� �ӽ� ó��.
            //var skillScore = 500;
            //_skillScore += skillScore;

            _player.Jump(skillIndex);
        }
    }

    /// <summary>
    /// ĳ���� �׶��ε� ����.
    /// </summary>
    public void PlayGrind()
    {
        if (_gameState == Enums.eGameState.Playing)
        {
            _player.Grind();
        }
    }

    /// <summary>
    /// ĳ���� �׶��ε� ����.
    /// </summary>
    public void EndGrind()
    {
        if (_gameState == Enums.eGameState.Playing)
        {
            _player.EndGrind();
        }
    }

    /// <summary>
    /// ��ų ��ư Ȱ��ȭ.
    /// </summary>
    /// <param name="skillIndexs"></param>
    public void SetReadySkill(List<int> skillIndexs)
    {
        _ingameUI.SetActiveSkillButton(skillIndexs);
    }

    /// <summary>
    /// �׶��ε� ��� ����.
    /// </summary>
    /// <param name="enable"></param>
    public void SetGrindMode(bool enable)
    {
        _ingameUI.SetGrindMode(enable);
    }
}
