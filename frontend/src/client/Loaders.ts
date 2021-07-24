import { SetState } from "@Palavyr-Types";

export class Loaders {
    private setIsLoading: SetState<boolean>;
    constructor(setIsLoading: SetState<boolean>) {
        this.setIsLoading = setIsLoading;
    }

    public startLoadingSpinner() {
        this.setIsLoading(true);
    }

    public stopLoadingSpinner() {
        this.setIsLoading(false);
    }

    public async LoadingWrapper<T>(request: any): Promise<T> {
        this.startLoadingSpinner();
        const result = await request();
        this.stopLoadingSpinner();
        return result;
    }
}
