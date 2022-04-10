using System.Collections;
using UnityEngine;
public class FishMaker : MonoBehaviour
{
    public Transform fishHolder;
    public Transform[] genPositions;
    public GameObject[] fishPrefabs;
    public float fishGenWatieTime=1f;//鱼生成的间隔和每隔5s生成一波鱼

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("MakeFishes", 5, fishGenWatieTime);
    }
    void MakeFishes()
    {
        int genPosIndex = Random.Range(0, genPositions.Length);
        int fishPreIndex = Random.Range(0, fishPrefabs.Length);//生成哪一种fish
        int maxNum = fishPrefabs[fishPreIndex].GetComponent<FishAttr>().maxNum;
        int maxSpeed = fishPrefabs[fishPreIndex].GetComponent<FishAttr>().maxSpeed;
        int num = Random.Range(maxNum / 2 + 1, maxNum);
        int speed = Random.Range(maxSpeed / 2, maxSpeed);
        int angOffset;  //仅直走生效,直走的倾斜角 
        angOffset = Random.Range(-22, 22);
        StartCoroutine(GenStraightFish(genPosIndex, fishPreIndex, num, speed, angOffset));//不能直接调用IEnumerator GenStraightFish方法
                                                                                              //直走鱼的生成
        
    }
    IEnumerator GenStraightFish(int genPosIndex, int fishPreIndex, int num, int speed, int angOffset)
    {//IEnumerator是这个using System.Collections;返回值类型
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishPreIndex]);
            fish.transform.SetParent(fishHolder, false);
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.transform.Rotate(0, 0, angOffset);//倾斜角
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;//解决unity渲染序列问题，即每生成一条鱼就让它的层+i
            fish.AddComponent<AutoMove>().speed = speed;
            yield return new WaitForSeconds(fishGenWatieTime);//返回一个IEnumerator类型值，等待相应的秒数fishGenWatieTime再去运行函数GenStraightFish
                                                              //每生成一条鱼等待X秒再生成下一条

        }
    }
   

}
