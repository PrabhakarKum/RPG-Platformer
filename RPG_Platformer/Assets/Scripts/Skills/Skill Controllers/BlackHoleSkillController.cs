using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    private float _maxSize;
    private float _growSpeed;
    private float _shrinkSpeed;
    
    private bool _canGrow = true;
    private bool _canShrink;
    private bool _canCreateHotkeys = true;
    private bool _cloneAttackReleased;
    private bool _playerCanDisappear = true;
    
    private int _amountOfAttacks = 4;
    private float _cloneAttackCooldown = 0.3f;
    private float _cloneAttackTimer;
    private float _blackHoleTimer;

    private List<Transform> _targets = new List<Transform>();
    private List<GameObject> _createdHotKeys = new List<GameObject>();
    
    public bool playerCanExitState { get; private set; }

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackHoleTimer)
    {
        this._maxSize = _maxSize;
        this._growSpeed = _growSpeed;
        this._shrinkSpeed = _shrinkSpeed;
        this._amountOfAttacks = _amountOfAttacks;
        this._cloneAttackCooldown = _cloneAttackCooldown;
        this._blackHoleTimer = _blackHoleTimer;
    }
    
    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime;
        _blackHoleTimer -= Time.deltaTime;

        if (_blackHoleTimer < 0)
        {
            _blackHoleTimer = Mathf.Infinity;
            if(_targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (_canGrow && !_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime );
        }

        if (_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed *Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if(_targets.Count <= 0)
            return;
        DestroyHotKey();
        _cloneAttackReleased = true;
        _canCreateHotkeys = false;
        if (_playerCanDisappear)
        {
            _playerCanDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
        
    }

    private void CloneAttackLogic()
    {
        if (_cloneAttackTimer < 0 && _cloneAttackReleased && _amountOfAttacks > 0)
        {
            _cloneAttackTimer = _cloneAttackCooldown;
            int randomIndex = Random.Range(0, _targets.Count);

            float xOffset;
            
            if (Random.Range(0, 100) > 50)
                xOffset = 0.2f;
            else
                xOffset = -0.2f;
            
            SkillManager.instance.cloneSkill.CreateClone(_targets[randomIndex], new Vector2(xOffset, 0));
            _amountOfAttacks--;

            if (_amountOfAttacks <= 0)
            {
               Invoke("FinishBlackHoleAbility",1f);
            }
                
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKey();
        playerCanExitState = true;
        _canShrink = true;
        _cloneAttackReleased = false;
    }

    private void DestroyHotKey()
    {
        if(_createdHotKeys.Count <= 0)
            return;

        for (int i = 0; i < _createdHotKeys.Count; i++)
        {
            Destroy(_createdHotKeys[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("No hotkey found in keycode list");
            return;
        }
        
        if(!_canCreateHotkeys)
            return;
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 0.4f) , Quaternion.identity);
        _createdHotKeys.Add(newHotKey);
            
        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);
            
            
        BlackHole_HotKey_Controller newHotKeyScript =  newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        newHotKeyScript.SetHotKey(chosenKey, collision.transform, this);
    }

    public void AddEnemiesToList(Transform _enemyTransform) => _targets.Add(_enemyTransform);
}
