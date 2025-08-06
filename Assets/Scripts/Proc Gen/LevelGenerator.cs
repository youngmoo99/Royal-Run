using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{  
   [Header("Refernces")]
   [SerializeField] CameraController cameraController;
   [SerializeField] GameObject chunkPrefab; //타일 프리팹

   [SerializeField] Transform chunkParent; 

   [Header("Level Settings")][Tooltip("The amount of chunks we start with")]
   [SerializeField] int startingChunksAmount = 12; //초기 생성할 chunk 수 
   [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")]
   [SerializeField] float chunkLength = 10f; // chunk 길이
   [SerializeField] float moveSpeed = 8f; // chunk가 앞으로 이동하는 속도
   [SerializeField] float minMoveSpeed = 2f; 
   [SerializeField] float maxMoveSpeed = 20f;
   [SerializeField] float minGravityZ = -22f; 
   [SerializeField] float maxGravityZ = -2f; 

   List<GameObject> chunks = new List<GameObject>(); // chunk 리스트 생성
    
    void Start()
    {
        SpawnStartingChunks(); // startingChunksAmount 만큼 chunk 생성
    }

    void Update()
    {
        MoveChunks(); // 매 프레임마다 chunk를 앞으로 이동시키고 화면 밖으로 나가면 제거 및 새 chunk 생성 
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {   
        float newMoveSpeed = moveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if(newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed;

            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ); // clamp 제한원래값, 최소값, 최대값 --> newGravityZ가 최소값보다 작으면 min값, 최대값보다 크면 max값, 그외의경우 원래값 그대로 반환 
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

            cameraController.ChangeCameraFOV(speedAmount);

        }


    }

    private void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ); 
        //Prefab 복제 Instantiate (gameobject, 위치, 회전여부, 부모설정)
        GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent); //인스턴스화될 때 자동으로 Transform chunkParent에추가됨 
        chunks.Add(newChunk); // 리스트 저장
    }

    private float CalculateSpawnPositionZ() //chunk의 z위치 계산 
    {
        float spawnPositionZ;

        if (chunks.Count == 0) // 첫 chunk는 현재 오브젝트 위치에 배치
        {
            spawnPositionZ = transform.position.z;
        }
        else // 이후는 이전 chunk보다 chunkLength 만큼 떨어지게 배치 
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ;
    }

    void MoveChunks()
    {
        for(int i = 0; i < chunks.Count; i++)
        {   
            GameObject chunk = chunks[i];
            //Translate(뒤쪽으로 이동) 현재 위치를 기준으로 상대적인 방향과 거리만큼 이동하는 함수 
            chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength) // 카메라 시야에서 사라지면 오브젝트 삭제 및 새 chunk 추가
            {   
                chunks.Remove(chunk);
                Destroy(chunk);
                SpawnChunk();
            }
        }
    }
}
