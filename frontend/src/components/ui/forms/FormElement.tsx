import * as React from "react"
import { PropsWithChildren } from "react"
import { ElementClassProps, InputStateProps } from "@/components/ui/forms/Types"

type FormElementProps = InputStateProps & ElementClassProps

const FormElement: React.FunctionComponent<PropsWithChildren<FormElementProps>> = ({hasErrors = false, required = false, elementWrapperClasses = "", children}: PropsWithChildren<FormElementProps>): JSX.Element => {

	const elementWrapperComputedClasses = `form__element ${elementWrapperClasses} ${hasErrors ? "form__element--has-error" : ""} ${required ? "form__element--required" : ""}`

	return (
		<div className={elementWrapperComputedClasses}>
			{children}
		</div>
	)
}

export default FormElement