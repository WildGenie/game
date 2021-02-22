import * as React from "react"
import { InputClassProps, InputProps, InputStateProps } from "@/components/ui/forms/Types"

type TextInputProps = InputProps<string> & InputStateProps & InputClassProps

const TextInput: React.FunctionComponent<TextInputProps> = ({
	name,
	value,
	setValue,
	focused = false,
	setFocused,
	hasErrors = false,
	required = false,
	type = "text",
	inputClasses = "",
	inputWrapperClasses = ""
}: TextInputProps): JSX.Element => {

	const inputWrapperComputedClasses = `form__input-wrapper ${inputWrapperClasses} ${!value && !focused ? "form__input-wrapper--empty" : ""} ${hasErrors ? "form__input-wrapper--has-error" : ""} ${focused ? "form__input-wrapper--focused" : ""} ${required ? "form__input-wrapper--required" : ""}`

	const inputComputedClasses = `form__input ${inputClasses} ${focused ? "form__input--focused" : ""} ${!value && !focused ? "form__input--empty" : ""} ${hasErrors ? "form__input--has-error" : ""} ${focused ? "form__input--focused" : ""} ${required ? "form__input--required" : ""}`

	return (
		<div className={inputWrapperComputedClasses}>
			<input
				id={name}
				className={inputComputedClasses}
				type={type}
				value={value}
				onFocus={() => setFocused && setFocused(true)}
				onBlur={() => setFocused && setFocused(false)}
				onChange={e => setValue(e.target.value)}
			/>
		</div>
	)
}

export default TextInput