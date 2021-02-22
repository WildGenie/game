import { FormFieldEntry } from "@/tools/definitions/forms"
import { CheckField, FormField } from "@/hooks/classes"

/**
 * Sets up the state and logic necessary for reusable form input validation
 *
 * @param {FormFieldEntry<T>>} field The form field for which to set up validation
 */
export function useFormInputValidation<T>(field: FormFieldEntry<T>): FormField<T> {
	return new FormField<T>(field.initialValue, field.validators)
}

export function useCheckInput<T>(value: T, checked: boolean): CheckField<T> {
	return new CheckField<T>(value, checked)
}