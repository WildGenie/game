import * as React from "react"
import { InputClassProps, InputProps, InputStateProps } from "@/components/ui/forms/Types"

type CheckInputProps = InputProps<boolean> & InputStateProps & InputClassProps

const CheckInput: React.FunctionComponent<CheckInputProps> = ({
	name,
	value,
	setValue,
	hasErrors = false,
	required = false,
	type = "checkbox",
	inputClasses = "",
	inputWrapperClasses = ""
}: CheckInputProps): JSX.Element => {

	const inputWrapperComputedClasses = `form__check-input-wrapper ${inputWrapperClasses} ${hasErrors ? "form__check-input-wrapper--has-error" : ""} ${required ? "form__check-input-wrapper--required" : ""}`

	const inputComputedClasses = `form__check-input ${inputClasses} ${hasErrors ? "form__check-input--has-error" : ""} ${required ? "form__check-input--required" : ""}`

	return (
		<div className={inputWrapperComputedClasses}>
			<input
				id={name}
				className={inputComputedClasses}
				type={type}
				checked={value}
				onChange={() => setValue(!value)}
			/>
		</div>
	)
}

export default CheckInput