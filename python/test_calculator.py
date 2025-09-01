import pytest
from calculator import calculate, tokenize, to_postfix, eval_postfix


# --------------------------
# ТЕСТЫ tokenize
# --------------------------

def test_tokenize_basic():
    assert tokenize("2 + 3") == ["2", "+", "3"]

def test_tokenize_with_parentheses():
    assert tokenize("( 10 - 2 ) * ( 3 + 1 )") == ["(", "10", "-", "2", ")", "*", "(", "3", "+", "1", ")"]

def test_tokenize_multiple_spaces():
    # Лишние пробелы не должны ломать (в простом варианте split всё равно разрежет корректно)
    expr = "  4   *   (  2 + 1 )  "
    assert tokenize(expr) == ["4", "*", "(", "2", "+", "1", ")"]

# --------------------------
# ТЕСТЫ to_postfix
# --------------------------

def test_postfix_simple_addition():
    assert to_postfix(["2", "+", "3"]) == ["2", "3", "+"]

def test_postfix_operator_precedence():
    # умножение должно идти раньше сложения
    assert to_postfix(["2", "+", "3", "*", "4"]) == ["2", "3", "4", "*", "+"]

def test_postfix_with_parentheses():
    assert to_postfix(["(", "2", "+", "3", ")", "*", "4"]) == ["2", "3", "+", "4", "*"]

def test_postfix_nested_parentheses():
    tokens = ["2", "*", "(", "3", "+", "(", "4", "-", "1", ")", ")"]
    assert to_postfix(tokens) == ["2", "3", "4", "1", "-", "+", "*"]

def test_postfix_unbalanced_parentheses():
    with pytest.raises(ValueError):
        to_postfix(["(", "2", "+", "3"])

# --------------------------
# ТЕСТЫ eval_postfix
# --------------------------

def test_eval_postfix_addition():
    assert eval_postfix(["2", "3", "+"]) == 5

def test_eval_postfix_mixed_operations():
    assert eval_postfix(["2", "3", "5", "*", "+"]) == 17  # 2 + 3*5
    assert eval_postfix(["10", "2", "-", "3", "+"]) == 11 # (10-2)+3

def test_eval_postfix_division():
    assert eval_postfix(["8", "2", "/"]) == 4
    assert eval_postfix(["7", "2", "/"]) == 3.5

def test_eval_postfix_chained():
    # (2 + 3) * (4 - 1) = 15
    postfix = ["2", "3", "+", "4", "1", "-", "*"]
    assert eval_postfix(postfix) == 15

def test_eval_postfix_error_not_enough_operands():
    with pytest.raises(ValueError):
        eval_postfix(["2", "+"])

def test_eval_postfix_error_unknown_token():
    with pytest.raises(ValueError):
        eval_postfix(["2", "X", "+"])

# --------------------------
# ТЕСТЫ calculate (интеграционные)
# --------------------------

def test_calculate_simple():
    assert calculate("2 + 3") == 5

def test_calculate_with_parentheses():
    assert calculate("2 + 5 * ( 3 - 7 )") == -18

def test_calculate_multiple():
    assert calculate("( 10 - 2 ) * ( 3 + 1 )") == 32

def test_calculate_division():
    assert calculate("8 / 2") == 4
    assert calculate("7 - 10 / 2") == 2  # проверка приоритета

def test_calculate_nested_parentheses():
    assert calculate("2 * ( 3 + ( 4 * ( 5 - 1 ) ) )") == 38

def test_calculate_negative_result():
    assert calculate("3 - 10") == -7

def test_calculate_multiple_operations():
    assert calculate("10 - 2 + 3 * 2") == 14

def test_calculate_large_expression():
    expr = "1 + 2 + 3 + 4 * 5 + ( 6 - 2 ) * 3"
    assert calculate(expr) == 38

def test_calculate_large_expression_2():
    expr = "1 + 2 * 3 + 4 * 5 + ( 6 - 2 ) * 3"
    assert calculate(expr) == 39


# --------------------------
# ТЕСТЫ на исключения
# --------------------------

def test_error_unbalanced_parentheses():
    with pytest.raises(ValueError):
        calculate("( 2 + 3")

def test_error_unknown_token():
    with pytest.raises(ValueError):
        to_postfix(["2", "?", "3"])

def test_error_not_enough_operands():
    with pytest.raises(ValueError):
        eval_postfix(["2", "+"])
