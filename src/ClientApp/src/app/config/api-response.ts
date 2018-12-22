export class ApiResponse {
    result: Object[];
    errorMessage: string;
    timeGenerated: string;

    isFailure() {
        if (this.errorMessage != null) {
            return true;
        }

        return false;
    }
}
