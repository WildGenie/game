import * as React from "react"
import { useEffect } from "react"
import {useState} from "react"

import { createGuid } from "@/tools/utils"
import FormErrorList from "@/components/ui/forms/FormErrorList"
import FormElement from "@/components/ui/forms/FormElement"
import FormLabel from "@/components/ui/forms/FormLabel"
import TextInput from "@/components/ui/forms/inputs/TextInput"
import Hint from "@/components/ui/forms/Hint"
import { ElementClassProps, HintClassProps, InputClassProps, InputProps, InputStateProps, LabelClassProps, LabelProps, NumberInputProps, ValidatableProps } from "@/components/ui/forms/Types"

type TextFieldTypes = {
	hint?: string
}
	& LabelProps
	& InputProps<string|number>
	& NumberInputProps
	& ValidatableProps
	& InputStateProps
	& ElementClassProps
	& LabelClassProps
	& InputClassProps
	& HintClassProps

const TextField = ({
	labelText,
	type,
	value,
	setValue,
	name = "",
	errors = [],
	validator = null,
	hint = "",
	required = false,
	step,
	min,
	max,
	elementWrapperClasses = "",
	labelWrapperClasses = "",
	labelClasses = "",
	inputWrapperClasses = "",
	inputClasses = "",
	hintWrapperClasses = "",
	hintClasses = ""
}: TextFieldTypes): JSX.Element => {
	// Hooks and other state
	const [inputName, setInputName] = useState(name)
	const [focused, setFocused] = useState(false)

	useEffect(() => {
		if (!inputName)
			setInputName(createGuid())
	})

	useEffect(() => { if (value && validator) validator() }, [value])
	
	const hasErrors = errors.length > 0

	// Computing classes

	const labelComputedClasses = `${labelClasses} ${focused ? "form__label--focused" : ""} ${type !== "number" && !value && !focused ? "form__label--empty" : ""}`

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
				focused={focused}
				labelClasses={labelComputedClasses}
				labelWrapperClasses={labelWrapperClasses}
			/>
			
			<TextInput
				name={inputName}
				value={value}
				setValue={setValue}
				focused={focused}
				setFocused={setFocused}
				type={type}
				step={step}
				min={min}
				max={max}
				inputClasses={inputClasses}
				inputWrapperClasses={inputWrapperClasses}
			/>

			<FormErrorList errors={errors}/>

			<Hint
				text={hint}
				hintClasses={hintClasses}
				hintWrapperClasses={hintWrapperClasses}
				focused={focused}
				hasErrors={hasErrors}
				required={required}
			/>
		</FormElement>
	)
}

export default TextField