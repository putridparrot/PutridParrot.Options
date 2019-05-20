export class Option {
}

class _None extends Option {    
}

export class Some<T> extends Option {
    constructor(private value: T) {        
        super();
    }

    public get Value() : T {
        return this.value;
    }    
}

export let None = new _None();

export class Options {
    public static toOption<T>(value: any): Option {
        return !Options.hasValue(value) ?
            None :  
            new Some<T>(value);
    }

    public static hasValue(value: any): boolean {
        return value != undefined && value != null ;
    }

    public static isOption(value: any): boolean {
        return value instanceof Option;
    }

    public static isSome(option: Option): boolean {
        return option instanceof Some;
    }

    public static isNone(option: Option): boolean {
        return option instanceof _None;
    }

    /**
     * Gets the value (if not undefined/null or None) else
     * returns the defaultValue
     * @param value 
     * @param defaultValue 
     */
    public static getValueOrDefault<T>(value: any | Option, defaultValue: any) {
        if(Options.hasValue(value)) {
            if(Options.isSome(value)) {
                let s = value as Some<T>;
                return s.Value;
            }
            else if(Options.isNone(value)) {
                return defaultValue; 
            }                
            return value;
        } 
        return defaultValue;
    }

    /**
     * If the Option is Some then invoke the supplied action
     * otherwise do nothing
     * @param option 
     * @param action 
     */
    public static do<T>(option: Option, action: (value: T) => void) {
        if(Options.isSome(option)) {
            let s = option as Some<T>;
            action(s.Value);
        }
    }

    /**
     * If the Option has Some value then call the
     * supplied function ro return a result else
     * return the noneValue
     * @param option 
     * @param someValue 
     * @param noneValue 
     */
    public static match<T, TResult>(option: Option, 
        someValue: (value: T) => TResult, 
        noneValue: TResult | any = null): TResult | any {
        if(Options.isSome(option)) {
            let s = option as Some<T>;
            return someValue(s.Value);
        }
        return noneValue;
    }

    /**
     * If the Option has a value and the boolean or predicate returns true then
     * return the option value otherwise returns None
     * @param option 
     * @param conditionOrPredicate 
     */
    public static if<T>(option: Option, conditionOrPredicate: boolean | ((value: T) => boolean)): Option {
        if(Options.isNone(option)) {
            return None;
        }

        let result = false;
        if(conditionOrPredicate instanceof Function) {
             let s = option as Some<T>;
             let f = conditionOrPredicate as (value: T) => boolean;
             result = f(s.Value);
        }
        else {
            result = conditionOrPredicate;
        }

        return result ? option : None;
    }

    /**
     * If the Option has a value then the value is returned otherwise
     * the supplied "other" value is returned via passed value or function
     * @param option 
     * @param other 
     */
    public static or<T>(option: Option, other: T | (() => T)): T {
        if(Options.isNone(option)) {
            if(other instanceof Function) {
                let f = other as () => T;
                return f();
            }
            return other;
        }

        let s = option as Some<T>;
        return s.Value;
    }

    /**
     * If the Option has no value then the errorFunc
     * is called to supply the Error to be thrown otherwise
     * the value is returned
     * @param option 
     * @param errorFunc 
     */
    public static orError<T>(option: Option, errorFunc: () => Error): T {
        if(Options.isNone(option)) {
            throw errorFunc();
        }
        
        let s = option as Some<T>;
        return s.Value;
    }
}