using Enums;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class Platform : MonoBehaviour
{
    private const float DEALY = 0.4f;
    private const float FADES_SPEED = 4;
    
    [SerializeField] internal PlatformGenerator _platformGenerator;

    [SerializeField] private Material _playerMaterial;

    internal bool _wasCollision;

    private bool _isFades;
    private float _timer;

    private void Update()
    {
        if (!_isFades) return;
        
        if (Timer()) return;
        
        FadesPlatformBeforDestraction();
    }

    private void FadesPlatformBeforDestraction()
    {
        foreach (var sector in _platformGenerator._thisPlatformSectors)
        {
            if(sector.CurrentType == SectorType.Null) continue;

            sector._renderer.shadowCastingMode = ShadowCastingMode.Off;
            
            var color = sector._renderer.material.color;
            
            color.a -= Time.deltaTime * FADES_SPEED;
            
            sector._renderer.material.color = color;
            
            if (color.a <= 0) _isFades = false;
        }
    }

    private bool Timer()
    {
        if (_timer < DEALY)
        {
            _timer += Time.deltaTime;
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
        if (!other.TryGetComponent(out Player player)) return;

        //записываем эту платформу в текущую игрка 
        player._currentPlatform = this;

        _wasCollision = false;

        Sector._numberOfSkippedPlatforms = player._numberOfSkippedPlatforms;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        player.NumberOfPassedPlatforms += PlayerPrefs.GetInt("LevelIndex") + 1;

        //если было столкновение обнуляет счетчик
        if (_wasCollision)
        {
            player._numberOfSkippedPlatforms = 0;
        }
        else
            //иначе считает пролеты
            player._numberOfSkippedPlatforms++;

        DestroyPlatform();
    }

    /// <summary>
    /// Разрушает платформу
    /// </summary>
    /// <param name="isHit"> При значении "true", сектора разлетаются вниз, как при ударе</param>
    public void DestroyPlatform(bool isHit = false)
    {
        for (int i = 0; i < _platformGenerator._thisPlatformSectors.Length; i++)
        {
            var impactStrength = Random.Range(4, 12);
            var scatteringStrength = Random.Range(6, 12);

            Rigidbody rigidbody = _platformGenerator._thisPlatformSectors[i].GameObject().GetComponent<Rigidbody>();
            Collider collider = _platformGenerator._thisPlatformSectors[i].GameObject().GetComponent<Collider>();
            Transform transform = _platformGenerator._thisPlatformSectors[i].GameObject().transform;

            collider.isTrigger = true;
            rigidbody.isKinematic = false;

            //получаем вектор из цента встороны(из-за начального разварота сектора от соответствует вектору вниз 
            Vector3 direction = transform.TransformDirection(Vector3.down);
            Vector3 torqueDirection = transform.TransformDirection(Vector3.right);

            if (isHit)
            {
                torqueDirection = transform.TransformDirection(Vector3.left);
                impactStrength *= -1;
                scatteringStrength = (int)(scatteringStrength * 0.5);

                _platformGenerator._thisPlatformSectors[i].GameObject().GetComponent<Renderer>().sharedMaterial =
                    _playerMaterial;
            }

            Vector3 forceAngle = new Vector3(scatteringStrength * direction.x, direction.y + impactStrength,
                scatteringStrength * direction.z);

            //Добавляем момент вращения секторам
            rigidbody.AddTorque(torqueDirection, ForceMode.Impulse);

            //Придаём импульс
            rigidbody.AddForce(forceAngle, ForceMode.Impulse);

            _isFades = true;
        }
    }
}