/* eslint-disable @typescript-eslint/ban-ts-comment */

import { describe, it, expect } from "@jest/globals"
import * as validations from "@/tools/validations"

const thrownErrorMessage = "Validation input must be a string"

describe("required", () => {

	const errorMessage = "Required"

	it("should return an error message if the input is null", () => {
		expect(validations.required(null))
			.toBe(errorMessage)
	})

	it("should return an error message if the input is undefined", () => {
		expect(validations.required(undefined))
			.toBe(errorMessage)
	})

	it("should return an error message if the input is false", () => {
		expect(validations.required(false))
			.toBe(errorMessage)
	})

	it("should return an error message if the input is an empty string", () => {
		expect(validations.required(""))
			.toBe(errorMessage)
	})

	it("should return an error message if the input is a whitespace string", () => {
		expect(validations.required("   "))
			.toBe(errorMessage)
	})

	it("should return an error message if the input is an empty array", () => {
		expect(validations.required([]))
			.toBe(errorMessage)
	})

	it("should return an error message if the input is an empty object", () => {
		expect(validations.required({}))
			.toBe(errorMessage)
	})

	it("should return undefined if the input is truthy", () => {
		expect(validations.required(true))
			.toBe(undefined)

		expect(validations.required("okay"))
			.toBe(undefined)

		expect(validations.required(1))
			.toBe(undefined)
	})

	it("should return undefined if the input is 0", () => {
		expect(validations.required(0))
			.toBe(undefined)
	})

})

describe("requireLength", () => {

	it("should throw an error if no input is supplied", () => {
		expect(() => validations.requireLength(undefined, undefined, undefined))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if input is not a string", () => {
		// @ts-ignore
		expect(() => validations.requireLength(0, 0, 0))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if max is not supplied", () => {
		expect(() => validations.requireLength("fails", 5, undefined))
			.toThrow("min and max options must both be numbers")
	})

	it("should throw an error if max is not a number", () => {
		// @ts-ignore
		expect(() => validations.requireLength("fails", 5, "really fails"))
			.toThrow("min and max options must both be numbers")
	})

	it("should throw an error if min is not supplied", () => {
		expect(() => validations.requireLength("fails", undefined, 5))
			.toThrow("min and max options must both be numbers")
	})

	it("should throw an error if min is not a number", () => {
		// @ts-ignore
		expect(() => validations.requireLength("fails", "really fails", 5))
			.toThrow("min and max options must both be numbers")
	})

	it("should return an error message if the input is too short", () => {
		expect(validations.requireLength("fails", 6, 10))
			.toBe("Must be between 6 and 10 characters")
	})

	it("should return an error message if the input is too long", () => {
		expect(validations.requireLength("failsfailsfails", 6, 10))
			.toBe("Must be between 6 and 10 characters")
	})

	it("should return undefined if the input is just right", () => {
		expect(validations.requireLength("failsfa", 6, 10))
			.toBe(undefined)
	})

})

describe("requireUpperCase", () => {

	it("should throw an error if no input is supplied", () => {
		expect(() => validations.requireUpperCase(undefined))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if input is not a string", () => {
		// @ts-ignore
		expect(() => validations.requireUpperCase(0))
			.toThrow(thrownErrorMessage)
	})

	it("should return an error message if the input does not contain an uppercase character", () => {
		expect(validations.requireUpperCase("no uppers here!"))
			.toBe("Must contain an uppercase letter")
	})

	it("should return undefined if the input contains an uppercase character", () => {
		expect(validations.requireUpperCase("some UPPERS here!"))
			.toBe(undefined)
	})

})

describe("requireLowerCase", () => {

	it("should throw an error if no input is supplied", () => {
		expect(() => validations.requireLowerCase(undefined))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if input is not a string", () => {
		// @ts-ignore
		expect(() => validations.requireLowerCase(0))
			.toThrow(thrownErrorMessage)
	})

	it("should return an error message if the input does not contain a lowercase character", () => {
		expect(validations.requireLowerCase("NO LOWERS HERE"))
			.toBe("Must contain a lowercase letter")
	})

	it("should return undefined if the input contains a lowercase character", () => {
		expect(validations.requireLowerCase("SOME lowers HERE!"))
			.toBe(undefined)
	})

})

describe("requireDigit", () => {

	it("should throw an error if no input is supplied", () => {
		expect(() => validations.requireDigit(undefined))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if input is not a string", () => {
		// @ts-ignore
		expect(() => validations.requireDigit(0))
			.toThrow(thrownErrorMessage)
	})

	it("should return an error message if the input does not contain a number", () => {
		expect(validations.requireDigit("No numbers here!"))
			.toBe("Must contain a number")
	})

	it("should return undefined if the input contains a number", () => {
		expect(validations.requireDigit("50m3 numb345 h343"))
			.toBe(undefined)
	})

})

describe("requireNoSpecialChar", () => {

	const validationErrorMessage = "Must contain only letters, numbers, and underscores"

	it("should throw an error if the input is not a string", () => {
		// @ts-ignore
		expect(() => validations.requireNoSpecialChar(5))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if the input is null", () => {
		expect(() => validations.requireNoSpecialChar(null))
			.toThrow(thrownErrorMessage)
	})

	it("should return an error message if the input contains a special character", () => {
		expect(validations.requireNoSpecialChar("5up3453c43t!"))
			.toBe(validationErrorMessage)
	})

	it("should return undefined if the input contains only lowercase letters", () => {
		expect(validations.requireNoSpecialChar("hello"))
			.toBe(undefined)
	})

	it("should return undefined if the input contains only uppercase letters", () => {
		expect(validations.requireNoSpecialChar("HELLO"))
			.toBe(undefined)
	})

	it("should return undefined if the input contains only letters and numbers", () => {
		expect(validations.requireNoSpecialChar("H3Ll0"))
			.toBe(undefined)
	})

})

describe("requireSpecialChar", () => {

	const validationErrorMessage = "Must contain at least one special character"

	it("should throw an error if the input is not a string", () => {
		// @ts-ignore
		expect(() => validations.requireSpecialChar(5))
			.toThrow(thrownErrorMessage)
	})

	it("should throw an error if the input is null", () => {
		expect(() => validations.requireSpecialChar(null))
			.toThrow(thrownErrorMessage)
	})

	it("should return an error message if the input contains only lowercase letters", () => {
		expect(validations.requireSpecialChar("hello"))
			.toBe(validationErrorMessage)
	})

	it("should return an error message if the input contains only uppercase letters", () => {
		expect(validations.requireSpecialChar("HELLO"))
			.toBe(validationErrorMessage)
	})

	it("should return an error message if the input contains only letters and numbers", () => {
		expect(validations.requireSpecialChar("H3Ll0"))
			.toBe(validationErrorMessage)
	})

	it("should return undefined if the input contains a special character", () => {
		expect(validations.requireSpecialChar("5up3453c43t!"))
			.toBe(undefined)
	})

})

describe("requireMatch", () => {

	it("should return the specified message if the inputs do not match", () => {
		expect(validations.requireMatch(5, 4, "Five is larger than four"))
			.toBe("Five is larger than four")
	})

	it("should return undefined if the inputs match", () => {
		expect(validations.requireMatch("hello", "hello", "Should not print"))
			.toBe(undefined)
	})

})