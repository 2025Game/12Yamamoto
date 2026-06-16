#ifndef CXPLAYER_H
#define CXPLAYER_H

#include "CXCharacter.h"
#include "CColliderLine.h"
#include "CPlayerIdle.h"
#include "CplayerWalk.h"

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
    std::unique_ptr<CPlayerWalk> mpWalk; //歩?状態
    CColliderLine mColliderLine;
    EState mState; //状態保持
    CState* mpState; //状態処理
    std::unique_ptr<CPlayerIdle> mpIdle; //待機状態
};

#endif