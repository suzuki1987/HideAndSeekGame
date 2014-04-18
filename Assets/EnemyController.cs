﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	private const float MOVE_SPEED_ADJUSTMENT = 0.05f;

	public	Transform	playerModelGameObjectTrans;
	public	Transform[] changeMoveEnemyPoint;
	public	float		fieldOfViewAngle		= 110f;				// Number of degrees, centred on forward, for the enemy see.
	//public	SphereCollider	col;

	private NavMeshAgent	agent;
	private bool			isPlayerInSight;
	private SphereCollider	col;
	private GameObject		playerGameObject;
	private	int				patrolIndex;

	void Start(){
		agent				= GetComponent<NavMeshAgent>();
		col					= GetComponent<SphereCollider>();
		playerGameObject	= GameObject.FindGameObjectWithTag(DoneTags.player);
		patrolIndex			= 0;
	}

	void Update(){
		if(isPlayerInSight){
			agent.SetDestination(playerGameObject.transform.position);
		}else{
			//agent.SetDestination(changeMoveEnemyPoint[0].position);
			Patrol();
		}
	}

	private void Patrol(){
		agent.SetDestination(changeMoveEnemyPoint[patrolIndex].position);
		float patrolPointX	= changeMoveEnemyPoint[patrolIndex].position.x;
		float patrolPointZ	= changeMoveEnemyPoint[patrolIndex].position.z;
		if(transform.position.x==patrolPointX && transform.position.z==patrolPointZ){
			patrolIndex ++;
			if(patrolIndex >= changeMoveEnemyPoint.Length){
				patrolIndex = 0;
			}
		}
	}

	void OnTriggerStay (Collider other){
		if(other.gameObject == playerGameObject){
			Vector3 direction	= other.transform.position - transform.position;
			float	angle		= Vector3.Angle(direction, transform.forward);
			
			if(angle < fieldOfViewAngle * 0.5f){
				RaycastHit	hit;
				var			layerMask		= 1<<8;
				bool		isFindPlayer	= Physics.Raycast(transform.position+transform.up, direction.normalized, out hit, col.radius, layerMask);
				if(isFindPlayer){
					if(hit.collider.gameObject == playerGameObject){
						isPlayerInSight = true;
					}
				}
			}
		}
	}

	void OnTriggerExit (Collider other){
	}
}
