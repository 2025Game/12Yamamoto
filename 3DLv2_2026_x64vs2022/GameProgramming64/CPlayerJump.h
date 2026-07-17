#pragma once

#include "CState.h"
#include "CVector.h"

class CPlayerJump : public CState
{
public:

    // 開始処理
    void Start(CXCharacter* parent) override;

    // 更新処理
    void Update() override;

    // 衝突処理
    void Collision(CCollider* m, CCollider* o) override;

private:

    CVector mJumpV; // ジャンプ速度
};