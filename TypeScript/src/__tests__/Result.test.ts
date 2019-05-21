import { Result, Success } from "../Result";

it('Result with Success', () => {
    let r : Result<string, number> = new Success<string, number>("Succeeded");
    
});