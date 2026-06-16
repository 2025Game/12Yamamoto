#include "CPlayerIdle.h"
#include "CXCharacter.h"
#include "CXPlayer.h"
//回転速度
#define ROTATIONSPEED 2.0f
void CPlayerIdle::Start(CXCharacter* parent)
{
	//親のポインタを保存
	mpParent = parent;
	//アニメーションの変更
	mpParent->ChangeAnimation(0, true, 60);
	mState = EState::EIDLE; //状態の種類を待機にする
}
void CPlayerIdle::Update()
{
  
   // Wキーが押されたら歩き状態
    if (mInput.Key('W'))
    {
        mState = EState::EWALK;
    }
}

