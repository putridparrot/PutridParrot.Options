import { Option, None, Some } from "../Option";

it('Option toOption with undefined, expect None', () => {

    let o = Option.toOption(undefined);

    expect(o).toBe(None);
});

it('Option toOption with null, expect None', () => {
    
    let o = Option.toOption(null);

    expect(o).toBe(None);
});

it('Option toOption with value, expect Some', () => {
    
    let o = Option.toOption("Hello World");
    
    expect(o).toBeInstanceOf(Some);
});

it('Option toOption with value, expect correct value from Some', () => {
    
    let o = Option.toOption("Hello World") as Some<string>;   
    
    expect(o.Value).toBe("Hello World");
});

it('Option isSome when it is None', () => {

    let o = Option.toOption(undefined);   

    expect(Option.isSome(o)).toBeFalsy();
});

it('Option isSome when it is Some', () => {
    
    let o = Option.toOption("Hello World");   

    expect(Option.isSome(o)).toBeTruthy();
});

it('Option getValueOrDefault with undefined, expect default', () => {
    
    expect(Option.getValueOrDefault(undefined, "Scooby")).toBe("Scooby");
});

it('Option getValueOrDefault with null, expect default', () => {
    
    expect(Option.getValueOrDefault(null, "Scooby")).toBe("Scooby");
});

it('Option getValueOrDefault with None, expect default', () => {
    
    expect(Option.getValueOrDefault(None, "Scooby")).toBe("Scooby");
});

it('Option getValueOrDefault with Some, expect value', () => {
    
    expect(Option.getValueOrDefault(new Some("Scooby"), "Doo")).toBe("Scooby");
});

it('Option do with None, expect no calls to action', () => {

    let a = jest.fn((s: string) => {});

    Option.do(None, a);

    expect(a).not.toHaveBeenCalled();
});

it('Option do with Some, expect action to be called', () => {

    let a = jest.fn((s: string) => {});

    Option.do(Option.toOption("Scooby"), a);

    expect(a).toHaveBeenCalledTimes(1);
});

it('Option ifElse with None, but no default, expect None', () => {

    let result = Option.ifElse<string, string | undefined>(None, value => undefined);

    expect(result).toBe(None);
});

it('Option ifElse with None, with default, expect default', () => {

    let result = Option.ifElse<string, string>(None, value => "", "Scooby");

    expect((result as Some<string>).Value).toBe("Scooby");
});

it('Option ifElse with Some, expect someValue function return', () => {

    let result = Option.ifElse<string, string>(Option.toOption("None"), value => "Scooby", "Doo");

    expect((result as Some<string>).Value).toBe("Scooby");
});

it('Option match with None, but no default, expect undefined', () => {

    let result = Option.match<string, string | undefined>(None, value => undefined);

    expect(result).toBe(undefined);
});

it('Option match with None, with default, expect default', () => {

    let result = Option.match<string, string>(None, value => "", "Scooby");

    expect(result).toBe("Scooby");
});

it('Option match with Some, expect someValue function return', () => {

    let result = Option.match<string, string>(Option.toOption("None"), value => "Scooby", "Doo");

    expect(result).toBe("Scooby");
});


it('Option if with None, expect None return', () => {

    let result = Option.if(None, true);

    expect(Option.isNone(result)).toBeTruthy();
});

it('Option if with Some and condition false, expect None return', () => {

    let result = Option.if(Option.toOption("Scooby"), false);

    expect(Option.isNone(result)).toBeTruthy();
});

it('Option if with Some and predicate false, expect None return', () => {

    let result = Option.if<string>(Option.toOption("Scooby"), (value: string) => { return false; });

    expect(Option.isNone(result)).toBeTruthy();
});

it('Option if with Some and condition true, expect Some return', () => {

    let result = Option.if(Option.toOption("Scooby"), true);

    expect(Option.isSome(result)).toBeTruthy();
});

it('Option if with Some and predicate true, expect Some return', () => {

    let result = Option.if<string>(Option.toOption("Scooby"), (value: string) => { return true; });

    expect(Option.isSome(result)).toBeTruthy();
});

it('Option orError with None expect Error', () => {

    const t = () => {
        Option.orError<string>(None, () => new Error());
    };

    expect(t).toThrow(Error);
});

it('Option orError with Some expect value', () => {

    let result = Option.orError<string>(Option.toOption("Scooby"), () => new Error());

    expect(result).toBe("Scooby");
});

