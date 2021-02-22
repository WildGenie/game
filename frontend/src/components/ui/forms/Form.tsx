import * as React from "react"
import {useState} from "react"

import Recaptcha from "@/components/ui/forms/fields/Recaptcha"
import { OnSubmitValidator, RecaptchaWindow } from "@/tools/definitions/forms"

import "./Form.scss"
import { Dispatch, SetStateAction } from "react"
import FormErrorList from "@/components/ui/forms/FormErrorList"

type FormProps = {
	children: React.ReactNode,
	handleSubmit: OnSubmitValidator,
	title: string,
	level?: number,
	showRecaptcha?: boolean,
	setRecaptchaValue?: Dispatch<SetStateAction<string>>,
	recaptchaErrors?: string[],
	recaptchaValue?: string,
	recaptchaValidator?: OnSubmitValidator,
	showErrors?: boolean,
	errors?: string[],
	showSubmit?: boolean,
	submitText?: string,
	showReset?: boolean,
	resetText?: string,
	showLegend?: boolean,
	validators?: OnSubmitValidator[],
	formClasses?: string
}

const Form: React.FunctionComponent<FormProps> = ({
	children,
	handleSubmit,
	title,
	level = 1,
	showRecaptcha = true,
	setRecaptchaValue,
	recaptchaErrors = [],
	recaptchaValue,
	recaptchaValidator,
	showErrors = false,
	errors = [],
	showSubmit = true,
	submitText = "Submit",
	showReset = true,
	resetText = "Reset",
	showLegend = true,
	validators = [],
	formClasses = ""
}: FormProps): JSX.Element => {

	const [recaptchaId, setRecaptchaId] = useState(-1)

	const onResetRecaptcha: VoidFunction = () => (window as unknown as RecaptchaWindow).grecaptcha.reset(recaptchaId)

	const HeadingTag = `h${level}` as keyof JSX.IntrinsicElements

	const validateInputs: OnSubmitValidator = () => {
		let hasErrors = false
		validators.forEach(validator => {
			// We put validator() first because we want
			// it to run no matter what
			// If validator() returns false but we still
			// have errors from a previous iteration,
			// we will still have that error state stored
			hasErrors = !validator() || hasErrors
		})

		// Need to return !hasErrors because OnSubmitValidators
		// return whether or not validation PASSES
		return !hasErrors
	}

	return (
		<form
			onSubmit={e => {
				e.preventDefault()
				validateInputs() && handleSubmit()
			}}
			onReset={() => onResetRecaptcha()}
			className={`form ${formClasses}`}
		>
			<HeadingTag>{title}</HeadingTag>

			{showLegend && (
				<p className="form__legend">= required</p>
			)}

			{children}

			{showRecaptcha && (
				<Recaptcha
					setValue={setRecaptchaValue}
					errors={recaptchaErrors}
					value={recaptchaValue}
					validator={recaptchaValidator}
					setRecaptchaId={setRecaptchaId}
				/>)
			}

			{showErrors && <FormErrorList errors={errors}/>}

			<div className="form__control-wrapper">
				{showSubmit && (
					<button
						className="form__control form__control--submit"
						type="submit"
					>
						{submitText}
					</button>
				)}
				{showReset && (
					<button
						className="form__control form__control--reset"
						type="reset"
					>
						{resetText}
					</button>
				)}
			</div>
		</form>
	)
}

export default Form