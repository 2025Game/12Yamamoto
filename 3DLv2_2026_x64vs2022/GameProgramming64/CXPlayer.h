#ifndef CXPLAYER_H
#define CXPLAYER_H

#include "CXCharacter.h"
#include "CColliderLine.h"
#include "CPlayerIdle.h"

class CXPlayer : public CXCharacter
{
public:
   
    CXPlayer();
    void Update() override;
    //衝突処理
//Collision(コライダ1, コライダ2)
    void Collision(CCollider* m, CCollider* o);
    //衝突処理
    void Collision();
private:
    CColliderLine mColliderLine;
    EState mState; //状態?保持
    CState* mpState; //状態処理
    std::unique_ptr<CPlayerIdle> mpIdle; //待機状態
};

#endif