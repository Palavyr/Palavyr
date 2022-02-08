import { SetState } from "@Palavyr-Types";

export class LogueModifier {
    // this modifies the prologue and epilogue (look to deprecate this in the future)

    onClick: SetState<string>;

    constructor(onClick: SetState<string>) {
        this.onClick = onClick;
    }

    simpleUpdateState(newLogue: string) {
        this.onClick(newLogue);
    }
}
