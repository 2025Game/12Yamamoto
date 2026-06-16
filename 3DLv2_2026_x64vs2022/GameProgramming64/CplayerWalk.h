#pragma once

#include "CState.h"
#include "CInput.h"

class CPlayerWalk : public CState
{
public:
    // 状態開始
    void Start(CXCharacter* parent) override;

    // 状態更新
    void Update() override;

private:
    CInput mInput;
};
