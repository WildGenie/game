import * as React from "react"
import {useHistory} from "react-router"

import Form from "@/components/ui/forms/Form"
import TextField from "@/components/ui/forms/fields/TextField"
import * as rules from "@/tools/validations"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { useFormInputValidation } from "@/hooks/forms"
import CheckField from "@/components/ui/forms/fields/CheckField"
import { register } from "@/tools/browser/users"
import { RegisterModel } from "@/tools/definitions/users"
import { getDocumentTitle } from "@/tools/utils"

const Register: React.FunctionComponent = (): JSX.Element => {
	const history = useHistory()

	const userName = useFormInputValidation<string>({
		initialValue: "",
		validators: [
			rules.required,
			(username: string) => rules.requireLength(username, 6, 32),
			rules.requireNoSpecialChar
		]
	}),
		email = useFormInputValidation<string>({
			initialValue: "",
			validators: [rules.required]
		}),
		password = useFormInputValidation<string>({
			initialValue: "",
			validators: [
				rules.required,
				(pw: string) => rules.requireLength(pw, 8, 64),
				rules.requireUpperCase,
				rules.requireLowerCase,
				rules.requireDigit,
				rules.requireSpecialChar
			]
		}),
		confirmPassword = useFormInputValidation<string>({
			initialValue: ""
		}),
		acceptTos = useFormInputValidation<boolean>({
			initialValue: false,
			validators: [rules.required]
		}),
		recaptchaToken = useFormInputValidation<string>({
			initialValue: "",
			validators: [rules.required]
		})

	confirmPassword.validators.push(() => rules.requireMatch(password.value, confirmPassword.value, "The passwords do not match"))

	const onSubmit: OnSubmitValidator = async () => {
		const data: RegisterModel = {
			email: email.value,
			userName: userName.value,
			password: password.value,
			confirmPassword: confirmPassword.value,
			recaptchaToken: recaptchaToken.value,
			acceptTos: acceptTos.value
		}
		const response = await register(data)

		if (response.wasSuccessful)
			history.push("/users/registration-successful")
		else {
			userName.setErrors(response.errors["UserName"])
			email.setErrors(response.errors["Email"])
			password.setErrors(response.errors["Password"])
			confirmPassword.setErrors(response.errors["ConfirmPassword"])
			acceptTos.setErrors(response.errors["AcceptTos"])
			recaptchaToken.setErrors(response.errors["RecaptchaToken"])
		}
		return true
	}

	document.title = getDocumentTitle("Register")

	return (
		<Form
			validators={[userName.validator, email.validator, password.validator, confirmPassword.validator, acceptTos.validator, recaptchaToken.validator]}
			handleSubmit={onSubmit}
			setRecaptchaValue={recaptchaToken.setValue}
			recaptchaErrors={recaptchaToken.errors}
			recaptchaValue={recaptchaToken.value}
			recaptchaValidator={recaptchaToken.validator}
			title="Register"
			formClasses="form--standalone"
		>
			<TextField
				labelText="Username"
				hint="Your username should be a mix of letters, numbers, and underscores between 6 and 32 characters"
				type="text"
				required={true}
				value={userName.value}
				setValue={userName.setValue}
				errors={userName.errors}
				validator={userName.validator}
			/>
			<TextField
				labelText="Email"
				type="email"
				required={true}
				value={email.value}
				setValue={email.setValue}
				errors={email.errors}
				validator={email.validator}
			/>
			<TextField
				labelText="Password"
				hint="Your password should be a mix of numbers, symbols, uppercase and lowercase letters at least 8 characters long"
				type="password"
				required={true}
				value={password.value}
				setValue={password.setValue}
				errors={password.errors}
				validator={password.validator}
			/>
			<TextField
				labelText="Confirm Password"
				type="password"
				required={true}
				value={confirmPassword.value}
				setValue={confirmPassword.setValue}
				errors={confirmPassword.errors}
				validator={confirmPassword.validator}
			/>
			<CheckField
				labelText={<>I accept the <a target="_blank" href="/terms">Terms of Service</a></>}
				value={acceptTos.value}
				setValue={acceptTos.setValue}
				errors={acceptTos.errors}
				validator={acceptTos.validator}
				required={true}
			/>
		</Form>
	)
}

export default Register