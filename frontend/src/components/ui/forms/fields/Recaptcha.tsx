import * as React from "react"
import { Dispatch, SetStateAction, useEffect } from "react"
import FormErrorList from "@/components/ui/forms/FormErrorList"
import { recaptchaSiteKey } from "@/tools/config"
import { OnSubmitValidator, RecaptchaWindow } from "@/tools/definitions/forms"

type RecaptchaProps = {
	setValue: Dispatch<SetStateAction<string>>,
	setRecaptchaId: Dispatch<SetStateAction<number>>,
	errors: string[],
	validator: OnSubmitValidator,
	value: string
}

const Recaptcha: React.FunctionComponent<RecaptchaProps> = ({setValue, setRecaptchaId, errors, validator, value}: RecaptchaProps): JSX.Element => {

	useEffect(() => { validator && validator() }, [value])

	useEffect(() => {
		const w = window as unknown as RecaptchaWindow
		const onRecaptchaReady: VoidFunction = () => {
			// Recaptcha script hasn't loaded yet
			// Give it some time and retry
			if (!w.grecaptcha) {
				setTimeout(onRecaptchaReady, 500)
				return
			}

			// Recaptcha script has loaded
			// Set up
			w.grecaptcha.ready(() => {
				const widget = w.grecaptcha.render("g-recaptcha", {
					sitekey: recaptchaSiteKey,
					callback: response => {
						setValue(response)
					}
				})

				setRecaptchaId(widget)
			})
		}

		onRecaptchaReady()
	}, [])

	return (
		<div className="form__element">
			<div className="form__recaptcha-wrapper">
				<div
					id="g-recaptcha"
					className="g-recaptcha form__recaptcha"
				/>
				<FormErrorList errors={errors}/>
			</div>
		</div>
	)
}

export default Recaptcha