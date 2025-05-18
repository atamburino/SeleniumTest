Feature: Simple math
  As a user
  I want to verify that addition works

  Scenario: Add two numbers
    Given I have entered 2 into the calculator
    And I have entered 3 into the calculator
    When I press add
    Then the result should be 5 