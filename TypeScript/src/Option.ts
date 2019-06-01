/**
 * Option base type. An Option can be a None or Some
 */
export abstract class Option {

    /**
     * Takes a value of any type and creates an Option
     * from it, undefined/null becomes a None whilst
     * other values are wrapped as Some
     * @param value 
     */
    public static toOption<T>(value: any): Option {
        return !Option.hasValue(value) ?
            None :  
            new Some<T>(value);
    }

    /**
     * Gets whether a value exists, i.e. it's
     * neither undefined or null
     * @param value 
     */
    public static hasValue(value: any): boolean {
        return value !== undefined && value !== null;
    }

    /**
     * Gets whether the value is a Some
     * @param value 
     */
    public static isSome(value: any): boolean {
        return value instanceof Some;
    }

    /**
     * Gets whether the value is a None
     * @param value 
     */
    public static isNone(value: any): boolean {
        return value instanceof NoneOption;
    }

    /**
     * Gets the value (if not undefined/null or None) else
     * returns the defaultValue. This unwraps an Option
     * @param value 
     * @param defaultValue 
     */
    public static getValueOrDefault<T>(value: any | Option, defaultValue: any): T {
        if(Option.hasValue(value)) {
            if(Option.isSome(value)) {
                let s = value as Some<T>;
                return s.Value;
            }
            else if(Option.isNone(value)) {
                return defaultValue; 
            }                
            return value;
        } 
        return defaultValue;
    }

    /**
     * If the Option is Some then invoke the supplied action
     * with passing the Some.Value, otherwise do nothing
     * @param option 
     * @param action 
     */
    public static do<T>(option: Option, action: (value: T) => void): void {
        if(Option.isSome(option)) {
            let s = option as Some<T>;
            action(s.Value);
        }
    }

    /**
     * If the Option has Some.Value then call the
     * supplied function returning its result else
     * return the noneValue. Differs from ifElse in
     * that the unwrapped value is returned
     * @param option 
     * @param someValue 
     * @param noneValue 
     */
    public static match<T, TResult>(option: Option, 
        someValue: (value: T) => TResult, 
        noneValue: TResult | any = undefined): TResult | any {
        if(Option.isSome(option)) {
            let s = option as Some<T>;
            return someValue(s.Value);
        }
        return noneValue;
    }

    /**
     * If the Option has Some value then call the
     * supplied function returning its result as an Option 
     * else return the noneValue as an Option
     * @param option 
     * @param someValue 
     * @param noneValue 
     */
    public static ifElse<T, TResult>(option: Option, 
        someValue: (value: T) => TResult, 
        noneValue: TResult | any = null): Option {

        let result : TResult;
        if(Option.isSome(option)) {
            let s = option as Some<T>;
            result = someValue(s.Value);
        }
        else {
            result = noneValue;
        }

        return Option.toOption(result);
    }

    /**
     * If the Option has a value and the boolean or predicate returns true then
     * return the option value otherwise returns None
     * @param option 
     * @param conditionOrPredicate 
     */
    public static if<T>(option: Option, conditionOrPredicate: boolean | ((value: T) => boolean)): Option {
        if(Option.isNone(option)) {
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
     * If the Option has no value then the errorFunc
     * is called to supply the Error to be thrown otherwise
     * the value is returned
     * @param option 
     * @param errorFunc 
     */
    public static orError<T>(option: Option, errorFunc: () => Error): T {
        if(Option.isNone(option)) {
            throw errorFunc();
        }
        
        let s = option as Some<T>;
        return s.Value;
    }}

/**
 * A None Option type
 */
class NoneOption extends Option {    
}

/**
 * A Some object has a non-null/non-undefined value
 */
export class Some<T> extends Option {
    public constructor(private value: T) {        
        super();
    }

    public get Value(): T {
        return this.value;
    }    
}

/**
 * A module/singleton for a None value
 */
export let None = new NoneOption();

