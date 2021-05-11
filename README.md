<summary>
    Given algorithm implements several steps, including:
    1. constructing binary matrix B(separate for every truth table result column):
        for given boolean function f: [f] = [f', f''],
            with [f'] = [f(0), ... ,f(2^(n-1))] and
                [f''] = [f(2 ^ (n - 1)), ... , f((2 ^ n) - 1)]
                    then b[f] is defined as:
                                | B[f']  B[f''] |
                        B[f] =  | B[f''] B[f']  |   and B[f(i)] = f(i);
    2.constructing binary matrix Z:
        Z ^ n is defined as n - th Kronecker - power of | 1 1 |
                                                        | 0 1 |
    3. Obtaining matrix M which is calculated by the following formula:
        (B[f] * Z ^ n) mod 2 = M[f]
    4. For every row of matrix obtained, select one with least nonzero coefficients.
        the number of row defines optimum polarity,
            allowing us to get the shortest polynomial for given vector f.

*for detailed explanation and further examples, please see / doc / harking_algorithm
</summary>