#include "CPlayerAttack.h"
#include "CXCharacter.h"


void CPlayerAttack::Start(CXCharacter* parent)
{
	//親ポインタを保存
	mpParent = parent;

	//攻撃アニメーション再生
	//番号3、繰り返しなし、30フレーム
	mpParent->ChangeAnimation(3, false, 30);

	//状態を攻撃にする
	mState = EState::EATTACK;
}


void CPlayerAttack::Update()
{
	//アニメーションが終了しているか
	if (mpParent->IsAnimationFinished())
	{
		//終了したら待機状態へ
		mState = EState::EIDLE;
	}
}