import * as React from "react"
import { useEffect, useState } from "react"
import { createGuid } from "@/tools/utils"
import FormElement from "@/components/ui/forms/FormElement"
import FormLabel from "@/components/ui/forms/FormLabel"
import FormErrorList from "@/components/ui/forms/FormErrorList"
import { ElementClassProps, InputClassProps, InputProps, InputStateProps, LabelClassProps, LabelProps, ValidatableProps } from "@/components/ui/forms/Types"
import CheckInput from "@/components/ui/forms/inputs/CheckInput"

type CheckInputTypes = LabelProps
	& InputProps<boolean>
	& ValidatableProps
	& InputStateProps
	& ElementClassProps
	& LabelClassProps
	& InputClassProps

const CheckField: React.FunctionComponent<CheckInputTypes> = ({
	labelText,
	name,
	value,
	setValue,
	errors = [],
	required = false,
	elementWrapperClasses = "",
	labelWrapperClasses = "",
	labelClasses = "",
	inputWrapperClasses = "",
	inputClasses = ""
}: CheckInputTypes): JSX.Element => {

	const [inputName, setInputName] = useState(name)

	useEffect(() => {
		if (!inputName) {
			setInputName(createGuid())
		}
	}, [])

	const hasErrors = errors.length > 0

	return (
		<FormElement
			hasErrors={hasErrors}
			required={required}
			elementWrapperClasses={`${elementWrapperClasses} form__element--check`}
		>
			<div className="form__check-field-wrapper">
				<FormLabel
					labelText={labelText}
					name={inputName}
					hasErrors={hasErrors}
					required={required}
					labelClasses={`${labelClasses} form__check-label`}
					labelWrapperClasses={`${labelWrapperClasses} form__check-label-wrapper`}
				/>

				<CheckInput
					value={value}
					setValue={setValue}
					name={inputName}
					hasErrors={hasErrors}
					required={required}
					inputClasses={`${inputClasses} form__check-input`}
					inputWrapperClasses={`${inputWrapperClasses} form__check-input-wrapper`}
				/>
			</div>

			<FormErrorList errors={errors}/>
		</FormElement>
	)
}

export default CheckField