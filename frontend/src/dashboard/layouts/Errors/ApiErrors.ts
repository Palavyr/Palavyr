import { SetState } from "@Palavyr-Types";
import { Errors } from "./ErrorPanel";

export class ApiErrors {
    private setSuccessOpen: SetState<boolean>;
    private setSuccessText: SetState<string>;
    private setWarningOpen: SetState<boolean>;
    private setWarningText: SetState<string>;
    private setErrorOpen: SetState<boolean>;
    private setErrorText: SetState<string>;
    private setErrors: SetState<Errors>;

    constructor(
        setSuccessOpen: SetState<boolean>,
        setSuccessText: SetState<string>,
        setWarningOpen: SetState<boolean>,
        setWarningText: SetState<string>,
        setErrorOpen: SetState<boolean>,
        setErrorText: SetState<string>,
        setErrors: SetState<Errors>
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
        this.setSuccessOpen(true);
        this.setSuccessText(message ?? "Success");
    }

    public SetWarningSnack(message?: string) {
        this.setWarningOpen(true);
        this.setWarningText(message ?? "Warning!");
    }

    public SetErrorSnack(message?: string) {
        this.setErrorOpen(true);
        this.setWarningText(message ?? "Error");
    }

    public SetErrorPanel(errors: Errors) {
        this.setErrors(errors);
    }
}
