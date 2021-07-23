import { PanelErrors, SetState } from "@Palavyr-Types";

export class ApiErrors {
    private setSuccessOpen: SetState<boolean>;
    private setSuccessText: SetState<string>;
    private setWarningOpen: SetState<boolean>;
    private setWarningText: SetState<string>;
    private setErrorOpen: SetState<boolean>;
    private setErrorText: SetState<string>;
    private setErrors: SetState<string[]>;

    constructor(
        setSuccessOpen: SetState<boolean>,
        setSuccessText: SetState<string>,
        setWarningOpen: SetState<boolean>,
        setWarningText: SetState<string>,
        setErrorOpen: SetState<boolean>,
        setErrorText: SetState<string>,
        setErrors: SetState<string[]>
    ) {
        this.setSuccessOpen = setSuccessOpen;
        this.setSuccessText = setSuccessText;
        this.setWarningOpen = setWarningOpen;
        this.setWarningText = setWarningText;
        this.setErrorOpen = setErrorOpen;
        this.setErrorText = setErrorText;
        this.setErrors = setErrors;
    }

    public SetSuccessSnack(message?: string) {
        this.setSuccessText(message ?? "Success");
        this.setSuccessOpen(true);
    }

    public SetWarningSnack(message?: string) {
        this.setWarningText(message ?? "Warning!");
        this.setWarningOpen(true);
    }

    public SetErrorSnack(message?: string) {
        this.setErrorText(message ?? "Error");
        this.setErrorOpen(true);
    }

    public SetErrorPanel(errors: string[]) {
        this.setErrors(errors);
    }
}
