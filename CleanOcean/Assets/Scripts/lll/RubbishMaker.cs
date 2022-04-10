using System.Collections;
using UnityEngine;
public class RubbishMaker : MonoBehaviour {
	public Transform rubbishHolder;
	public Transform[] genRPositions;
	public GameObject[] rubbishPrefabs;
    public float waveGenWatieTime = 0.3f;//每0.3s生成一波垃圾
    public float rubbishGenWatieTime = 2f;//每种垃圾生成的间隔

    // Use this for initialization
    void Start () {
        InvokeRepeating("MakeFishes", 0,waveGenWatieTime);
	}
	void MakeFishes(){
		int genPosIndex = Random.Range (0, genRPositions.Length);  //随机生成位置
		int rubbishPreIndex = Random.Range (0, rubbishPrefabs.Length);//随机生成哪一种垃圾
		int maxNum = rubbishPrefabs [rubbishPreIndex].GetComponent<FishAttr> ().maxNum;
		int maxSpeed = rubbishPrefabs [rubbishPreIndex].GetComponent<FishAttr> ().maxSpeed;
		int num = Random.Range (maxNum / 2 + 1, maxNum);
		int speed = Random.Range (maxSpeed / 2, maxSpeed);
		int moveType = Random.Range (0, 2);   //0:直走，1:转弯
		int angOffset;  //仅直走生效,直走的倾斜角 
		int angSpeed;   //仅转弯生效,转弯的角速度

		if (moveType ==0) {
			angOffset = Random.Range (-22, 22);
			StartCoroutine(GenStraightFish(genPosIndex,rubbishPreIndex,num,speed,angOffset));//不能直接调用IEnumerator GenStraightFish方法
                                                                                          //直走鱼的生成
        } 
		else {
            //TODO 转弯鱼群
            if (Random.Range(0,2) == 0)//是否取负的角速度
            {
                angSpeed = Random.Range(-15, -9);
            }
            else
            {
                angSpeed = Random.Range(9, 15);
            }
            StartCoroutine(GenTurnFish(genPosIndex, rubbishPreIndex, num, speed, angSpeed));//不能直接调用IEnumerator GenStraightFish方法

        }
    }
	IEnumerator GenStraightFish(int genPosIndex,int fishPreIndex,int num,int speed,int angOffset)
    {//IEnumerator是这个using System.Collections;返回值类型
        for (int i = 0; i < num; i++) {
			GameObject fish = Instantiate(rubbishPrefabs[fishPreIndex]);
			fish.transform.SetParent(rubbishHolder,false);
			fish.transform.localPosition = genRPositions[genPosIndex].localPosition;
			fish.transform.localRotation = genRPositions[genPosIndex].localRotation;
			fish.transform.Rotate (0, 0, angOffset);//倾斜角
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;//解决unity渲染序列问题，即每生成一个垃圾就让它的层+i
            fish.AddComponent<AutoMove>().speed = speed;
            yield return new WaitForSeconds(rubbishGenWatieTime);//返回一个IEnumerator类型值，等待相应的秒数rubbishGenWatieTime再去运行函数GenStraightFish
                                                              //每生成一个垃圾等待X秒再生成下一个

        }
	}
    IEnumerator GenTurnFish(int genPosIndex, int fishPreIndex, int num, int speed, int angSpeed)
    {//IEnumerator是这个using System.Collections;返回值类型
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(rubbishPrefabs[fishPreIndex]);
            fish.transform.SetParent(rubbishHolder, false);
            fish.transform.localPosition = genRPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genRPositions[genPosIndex].localRotation;
            fish.GetComponent<SpriteRenderer>().sortingOrder += (i+1);//解决unity渲染序列问题，即每生成一个垃圾就让它的层+i
            fish.AddComponent<AutoMove>().speed = speed;
            fish.AddComponent<AutoRotation>().speed = angSpeed;
            yield return new WaitForSeconds(rubbishGenWatieTime);//返回一个IEnumerator类型值，等待相应的秒数fishGenWatieTime再去运行函数GenStraightFish
                                                              //每生成一个垃圾等待X秒再生成下一个

        }
    }

}
