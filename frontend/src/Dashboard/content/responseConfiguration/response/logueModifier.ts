import { Dispatch, SetStateAction } from "react";


export class LogueModifier {
    // this modifies the prologue and epilogue (look to deprecate this in the future)

    onClick: Dispatch<SetStateAction<string>>

    constructor(onClick: Dispatch<SetStateAction<string>>) {
        this.onClick = onClick;
    }

    simpleUpdateState(newLogue: string) {
        this.onClick(newLogue)
    }
}

