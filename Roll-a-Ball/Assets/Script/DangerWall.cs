using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DangerWall : MonoBehaviour
{
	//オブジェクトと接触したときに呼ばれるコールバック
	void OnCollisionEnter( Collision hit )
	{
		//接触したオブジェクトのタグがPlayerの場合
		if( hit.gameObject.CompareTag( "Player" ) ){
			//現在のシーン番号を取得
			int sceneIndex = SceneManager.GetActiveScene().buildIndex;
			
			//現在のシーンを再読み込みする
			SceneManager.LoadScene( sceneIndex );
		}
	}
}
