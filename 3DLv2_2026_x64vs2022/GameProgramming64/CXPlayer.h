#ifndef CXPLAYER_H
#define CXPLAYER_H

#include "CXCharacter.h"
#include "CColliderLine.h"

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
   
};

#endif