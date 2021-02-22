import { Dispatch, SetStateAction, useState } from "react"
import { FieldValidator, OnSubmitValidator } from "@/tools/definitions/forms"

export class FormField<T> {
	value: T
	setValue: Dispatch<SetStateAction<T>>
	errors: string[]
	setErrors: Dispatch<SetStateAction<string[]>>
	validators: FieldValidator<T>[]
	validator: OnSubmitValidator
	constructor(initialValue: T, validators: FieldValidator<T>[] = []) {
		[this.value, this.setValue] = useState(initialValue);
		[this.errors, this.setErrors] = useState([])
		this.validators = validators

		this.validator = (): boolean => {
			// If there are no validators, we can't run validation
			if (this.validators.length === 0)
				return true

			const validationMessages: string[] = []

			this.validators.forEach(v => {
				const result = v(this.value)
				if (result)
					validationMessages.push(result)
			})

			this.setErrors(validationMessages)

			return validationMessages.length === 0
		}
	}
}

export class CheckField<T> {
	value: T
	checked: boolean
	setChecked: Dispatch<SetStateAction<boolean>>
	constructor(initialValue: T, checked: boolean) {
		this.value = initialValue;
		[this.checked, this.setChecked] = useState(checked)
	}
}