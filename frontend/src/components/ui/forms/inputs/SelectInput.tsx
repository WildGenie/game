import * as React from "react"
import { InputClassProps, InputProps, InputStateProps } from "@/components/ui/forms/Types"
import { FormSelect } from "@/tools/definitions/forms"
import { useState } from "react"

type SelectInputProps = {
	options: FormSelect[]
} & InputProps<string|number> & InputStateProps & InputClassProps

const SelectInput: React.FunctionComponent<SelectInputProps> = ({
	options,
	name,
	value,
	setValue,
	inputClasses = "",
	inputWrapperClasses = ""
}: SelectInputProps): JSX.Element => {

	const [hasFocus, setHasFocus] = useState(false)

	const selectClasses = `form__select ${inputClasses} ${hasFocus ? "form__select--focused" : ""}`

	return (
		<div className={inputWrapperClasses}>
			<select
				id={name}
				value={value}
				onChange={e => setValue(options[e.target.selectedIndex].value)}
				onFocus={() => setHasFocus(true)}
				onBlur={() => setHasFocus(false)}
				className={selectClasses}
			>
				{options.map((option, i) => (
					<option
						key={i}
						value={option.value}
						className="form__select-option"
					>
						{option.text}
					</option>
				))}
			</select>
		</div>

	)
}

export default SelectInput