import * as React from "react"
import { useState, useEffect } from "react"
import FormElement from "@/components/ui/forms/FormElement"
import FormLabel from "@/components/ui/forms/FormLabel"
import SelectInput from "@/components/ui/forms/inputs/SelectInput"
import { ElementClassProps, HintClassProps, InputClassProps, InputProps, InputStateProps, LabelClassProps, LabelProps, ValidatableProps } from "@/components/ui/forms/Types"
import { createGuid } from "@/tools/utils"
import { FormSelect } from "@/tools/definitions/forms"

type SelectFieldProps = {
	options: FormSelect[]
} & LabelProps
	& InputProps<string|number>
	& ValidatableProps
	& InputStateProps
	& ElementClassProps
	& LabelClassProps
	& InputClassProps
	& HintClassProps

const SelectField: React.FunctionComponent<SelectFieldProps> = ({
	options,
	labelText,
	value,
	setValue,
	name = "",
	errors = [],
	required = false,
	elementWrapperClasses = "",
	labelWrapperClasses = "",
	labelClasses = "",
	inputWrapperClasses = "",
	inputClasses = "",
}: SelectFieldProps): JSX.Element => {

	const [inputName, setInputName] = useState(name)

	useEffect(() => {
		if (!inputName)
			setInputName(createGuid())
	})

	const hasErrors = errors.length > 0

	return (
		<FormElement
			hasErrors={hasErrors}
			required={required}
			elementWrapperClasses={elementWrapperClasses}
		>
			<FormLabel
				labelText={labelText}
				name={name}
				hasErrors={hasErrors}
				required={required}
				labelClasses={labelClasses}
				labelWrapperClasses={labelWrapperClasses}
			/>

			<SelectInput
				name={inputName}
				options={options}
				value={value}
				setValue={setValue}
				inputWrapperClasses={inputWrapperClasses}
				inputClasses={inputClasses}
			/>
		</FormElement>
	)
}

export default SelectField