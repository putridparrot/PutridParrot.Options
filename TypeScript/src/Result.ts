export abstract class Result<TSuccess, TFailure> {    
}

export class Success<TSuccess, TFailure> extends Result<TSuccess, TFailure> {
    public constructor(private value: TSuccess) {
        super();
    }

    public get Value(): TSuccess {
        return this.value;
    }    
}

export class Failure<TSuccess, TFailure> extends Result<TSuccess, TFailure> {
    public constructor(private value: TFailure) {
        super();
    }

    public get Value(): TFailure {
        return this.value;
    }      
}