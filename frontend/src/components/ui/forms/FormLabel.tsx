import * as React from "react"
import { InputStateProps, LabelClassProps, LabelProps } from "@/components/ui/forms/Types"

type FormLabelTypes = {
	name?: string
} & LabelProps & InputStateProps & LabelClassProps

const FormLabel: React.FunctionComponent<FormLabelTypes> = ({
	labelText,
	name,
	hasErrors,
	required,
	focused,
	labelClasses,
	labelWrapperClasses
}: FormLabelTypes): JSX.Element => {

	const labelWrapperComputedClasses = `form__label-wrapper ${labelWrapperClasses} ${focused ? "form__label-wrapper--focused" : ""} ${hasErrors ? "form__label-wrapper--has-error" : ""} ${required ? "form__label-wrapper--required" : ""}`

	const labelComputedClasses = `form__label ${labelClasses} ${focused ? "form__label--focused" : ""} ${hasErrors ? "form__label--has-error" : ""} ${required ? "form__label--required" : ""}`
	return (
		<div className={labelWrapperComputedClasses}>
			<label
				htmlFor={name}
				className={labelComputedClasses}>
				{labelText}
			</label>
		</div>
	)
}

export default FormLabel