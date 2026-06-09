#pragma once
class CCollider;
class CXCharacter;
//状態?種類
enum class EState
{
	ENONE, //状態?し
	EIDLE, //待機
	EWALK, //歩き
};
class CState
{
public:
	virtual ~CState() {};
	//状態?開始
	virtual void Start(CXCharacter* parent) {};
	//状態?更新
	virtual void Update() {};
	//衝突処理
	//Collision(コライダ1, コライダ2)
	virtual void Collision(CCollider* m, CCollider* o) {};
	//状態?取得
	EState State() { return mState; }
protected:
	EState mState; //状態?種類
	CXCharacter* mpParent; //親?ポインタ
}; 
