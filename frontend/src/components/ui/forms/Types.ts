import { Dispatch, SetStateAction, ReactNode } from "react"
import { OnSubmitValidator } from "@/tools/definitions/forms"

export type InputStateProps = {
	focused?: boolean,
	setFocused?: Dispatch<SetStateAction<boolean>>,
	hasErrors?: boolean,
	required?: boolean
}

export type LabelProps = {
	labelText: string | ReactNode
}

export type InputProps<T> = {
	value: T,
	setValue: Dispatch<SetStateAction<T>>,
	name?: string,
	type?: string
}

export type NumberInputProps = {
	step?: number,
	min?: number,
	max?: number
}

export type ValidatableProps = {
	errors?: string[],
	validator?: OnSubmitValidator
}

export type ElementClassProps = {
	elementWrapperClasses?: string
}

export type LabelClassProps = {
	labelWrapperClasses?: string,
	labelClasses?: string
}

export type InputClassProps = {
	inputWrapperClasses?: string,
	inputClasses?: string
}

export type HintClassProps = {
	hintWrapperClasses?: string,
	hintClasses?: string
}