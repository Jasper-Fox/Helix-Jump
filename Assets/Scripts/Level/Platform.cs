using UnityEngine;
using Unity.VisualScripting;
using Random = System.Random;

public class Platform : MonoBehaviour
{
    [SerializeField] private PlatformGenerator _platformGenerator;

    internal bool _wasCollision;

    private Random _random;

    private void OnTriggerEnter(Collider other)
    {
        //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
        if (other.TryGetComponent(out Player player))
        {
            //записываем эту платформу в текущую игрка 
            player._currentPlatform = this;
            _wasCollision = false;

            //записываем эту платформу в текущую всех секторов 
            for (int i = 0; i < _platformGenerator._thisPlatformSectors.Length; i++)
            {
                if (_platformGenerator._thisPlatformSectors[i] == null) return;

                _platformGenerator._thisPlatformSectors[i]._currentPlatform = this;
                _platformGenerator._thisPlatformSectors[i]._numberOfSkippedPlatforms = player._numberOfSkippedPlatforms;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        player.NumberOfPassedPlatforms += _platformGenerator._levelIndex + 1;

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

    public void DestroyPlatform(bool isHit = false)
    {
        for (int i = 0; i < _platformGenerator._thisPlatformSectors.Length; i++)
        {
            var ImpactStrength = UnityEngine.Random.Range(4, 12);
            var RachletStrength = UnityEngine.Random.Range(6, 12);
            
            Rigidbody rigidbody = _platformGenerator._thisPlatformSectors[i].GameObject().GetComponent<Rigidbody>();
            Collider collider = _platformGenerator._thisPlatformSectors[i].GameObject().GetComponent<Collider>();
            Transform transform = _platformGenerator._thisPlatformSectors[i].GameObject().transform;

            collider.isTrigger = true;
            rigidbody.isKinematic = false;

            Vector3 direction = transform.TransformDirection(Vector3.down);
            Vector3 torqueDirection = transform.TransformDirection(Vector3.right);
            
            if (isHit)
            {
                torqueDirection = transform.TransformDirection(Vector3.left);
                ImpactStrength *= -1;
                RachletStrength = (int)(RachletStrength * 0.5);
            }
            

            Vector3 forceAngle = new Vector3(RachletStrength * direction.x, direction.y + ImpactStrength,
                RachletStrength * direction.z);
            
            rigidbody.AddTorque(torqueDirection, ForceMode.Impulse);
            rigidbody.AddForce(forceAngle, ForceMode.Impulse);
        }
    }
}