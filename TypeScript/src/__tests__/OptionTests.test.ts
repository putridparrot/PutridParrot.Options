import { Options, None, Some } from "../Option";

it('Option toOption with undefined, expect None', () => {

    let o = Options.toOption(undefined);

    expect(o).toBe(None);
});

it('Option toOption with null, expect None', () => {
    
    let o = Options.toOption(null);

    expect(o).toBe(None);
});

it('Option toOption with value, expect Some', () => {
    
    let o = Options.toOption("Hello World");
    
    expect(o).toBeInstanceOf(Some);
});

it('Option toOption with value, expect correct value from Some', () => {
    
    let o = Options.toOption("Hello World") as Some<string>;   
    
    expect(o.Value).toBe("Hello World");
});

it('Option isSome when it is None', () => {

    let o = Options.toOption(undefined);   

    expect(Options.isSome(o)).toBeFalsy();
});

it('Option isSome when it is Some', () => {
    
    let o = Options.toOption("Hello World");   

    expect(Options.isSome(o)).toBeTruthy();
});

it('Option getValueOrDefault with undefined, expect default', () => {
    
    expect(Options.getValueOrDefault(undefined, "Scooby")).toBe("Scooby");
});

it('Option getValueOrDefault with null, expect default', () => {
    
    expect(Options.getValueOrDefault(null, "Scooby")).toBe("Scooby");
});

it('Option getValueOrDefault with None, expect default', () => {
    
    expect(Options.getValueOrDefault(None, "Scooby")).toBe("Scooby");
});

it('Option getValueOrDefault with Some, expect value', () => {
    
    expect(Options.getValueOrDefault(new Some("Scooby"), "Doo")).toBe("Scooby");
});

it('Option do with None, expect no calls to action', () => {

    let a = jest.fn((s: string) => {});

    Options.do(None, a);

    expect(a).not.toHaveBeenCalled();
});

it('Option do with Some, expect action to be called', () => {

    let a = jest.fn((s: string) => {});

    Options.do(Options.toOption("Scooby"), a);

    expect(a).toHaveBeenCalledTimes(1);
});

it('Option match with None, but no default, expect null', () => {

    let result = Options.match<string, string>(None, value => {return ""});

    expect(result).toBe(null);
});

it('Option match with None, with default, expect default', () => {

    let result = Options.match<string, string>(None, value => {return ""}, "Scooby");

    expect(result).toBe("Scooby");
});

it('Option match with Some, expect someValue function return', () => {

    let result = Options.match<string, string>(Options.toOption("None"), value => {return "Scooby"}, "Doo");

    expect(result).toBe("Scooby");
});

it('Option if with None, expect None return', () => {

    let result = Options.if(None, true);

    expect(Options.isNone(result)).toBeTruthy();
});

it('Option if with Some and condition false, expect None return', () => {

    let result = Options.if(Options.toOption("Scooby"), false);

    expect(Options.isNone(result)).toBeTruthy();
});

it('Option if with Some and predicate false, expect None return', () => {

    let result = Options.if<string>(Options.toOption("Scooby"), (value: string) => { return false; });

    expect(Options.isNone(result)).toBeTruthy();
});

it('Option if with Some and condition true, expect Some return', () => {

    let result = Options.if(Options.toOption("Scooby"), true);

    expect(Options.isSome(result)).toBeTruthy();
});

it('Option if with Some and predicate true, expect Some return', () => {

    let result = Options.if<string>(Options.toOption("Scooby"), (value: string) => { return true; });

    expect(Options.isSome(result)).toBeTruthy();
});

it('Option or with None expect other return', () => {

    let result = Options.or<string>(None, "Scooby");

    expect(result).toBe("Scooby");
});

it('Option or with None expect other function return', () => {

    let result = Options.or<string>(None, () => "Scooby");

    expect(result).toBe("Scooby");
});

it('Option or with Some expect some return', () => {

    let result = Options.or<string>(Options.toOption("Scooby"), "Doo");

    expect(result).toBe("Scooby");
});

it('Option orError with None expect Error', () => {

    const t = () => {
        Options.orError<string>(None, () => new Error());
    };

    expect(t).toThrow(Error);
});

it('Option orError with Some expect value', () => {

    let result = Options.orError<string>(Options.toOption("Scooby"), () => new Error());

    expect(result).toBe("Scooby");
});
