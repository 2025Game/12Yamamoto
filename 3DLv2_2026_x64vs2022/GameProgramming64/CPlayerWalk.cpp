#include "CplayerWalk.h"
#include "CXCharacter.h"
#define ROTATIONSPEED 2.0f



void CPlayerWalk::Start(CXCharacter* parent)
{
	//親のポインタを保存
	mpParent = parent;
	//アニメーションの変更
	mpParent->ChangeAnimation(1, true, 60);
	mState = EState::EWALK; //状態の種類を歩くにする
}
//移動速度
#define VELOCITY 0.1f
void CPlayerWalk::Update()
{
	
		// Wキーで前進
		if (mInput.Key('W'))
		{
			CVector r = mpParent->Position();

			mpParent->Position(r +
				mpParent->MatrixRotate().VectorZ() * VELOCITY);
		}
		else
		{
			//Wがないと待機
			mState = EState::EIDLE;
		}
		// Aキーで左回転
		if (mInput.Key('A'))
		{
			CVector r = mpParent->Rotation() +
				CVector(0.0f, ROTATIONSPEED, 0.0f);

			mpParent->Rotation(r);
		}


		// Dキーで右回転
		if (mInput.Key('D'))
		{
			CVector r = mpParent->Rotation() +
				CVector(0.0f, -ROTATIONSPEED, 0.0f);

			mpParent->Rotation(r);
		}
		// Iキーが押されたら攻撃状態
		if (mInput.Key('I'))
		{
			mState = EState::EATTACK;
		}

}
	