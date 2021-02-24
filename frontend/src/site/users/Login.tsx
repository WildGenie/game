import * as React from "react"
import { Dispatch, SetStateAction, useState } from "react"
import { Link, useHistory } from "react-router-dom"
import { useSelector, useDispatch } from "react-redux"

import Form from "@/components/ui/forms/Form"
import { useFormInputValidation } from "@/hooks/forms"
import { required } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import CheckField from "@/components/ui/forms/fields/CheckField"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { login } from "@/tools/browser/users"
import { LoginModel, LoginModelErrorKeys } from "@/tools/definitions/users"
import { getCurrentUser } from "@/site/currentUser"
import { login as doLogin } from "@/site/currentUser"
import { getDocumentTitle } from "@/tools/utils"

const Login: React.FunctionComponent = (): JSX.Element => {

	const history = useHistory()
	const dispatch = useDispatch()
	const {isLoggedIn}: {isLoggedIn: boolean} = useSelector(getCurrentUser)

	const [rememberMe, setRememberMe] = useState(false)
	const [recaptchaRequired, setRecaptchaRequired] = useState(false)
	const [unknownErrors, setUnknownErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])

	const accountName = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})
	const password = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})
	const recaptchaToken = useFormInputValidation<string>({ initialValue: "" })

	const onLogin: OnSubmitValidator = async () => {
		const loginData: LoginModel = {
			accountName: accountName.value,
			password: password.value,
			rememberMe
		}
		if (recaptchaRequired)
			loginData.recaptchaToken = recaptchaToken.value

		const response = await login(loginData)

		if (response.wasSuccessful) {
			dispatch(doLogin(response.result))
			history.push("/")
		}
		else {
			accountName.setErrors(response.errors[LoginModelErrorKeys.accountName])
			recaptchaToken.setErrors(response.errors[LoginModelErrorKeys.recaptchaToken])
			setUnknownErrors(response.errors["Unknown"])
			if (response.errors[LoginModelErrorKeys.recaptchaToken]
				&& response.errors[LoginModelErrorKeys.recaptchaToken].length > 0)
				setRecaptchaRequired(true)
		}
		return true
	}

	document.title = getDocumentTitle("Log in")

	return (
		<>
			{!isLoggedIn && (
				<Form
					handleSubmit={onLogin}
					title="Login"
					formClasses="form--standalone"
					showRecaptcha={recaptchaRequired}
					showReset={false}
					recaptchaValue={recaptchaToken.value}
					setRecaptchaValue={recaptchaToken.setValue}
					recaptchaErrors={recaptchaToken.errors}
					showErrors={true}
					errors={unknownErrors}
				>
					<p className="app__helpful-text">
						Need an account? <Link to="/users/register">Create one now</Link>
					</p>
					<TextField
						labelText="Account name"
						hint="Your username or email address"
						value={accountName.value}
						setValue={accountName.setValue}
						validator={accountName.validator}
						errors={accountName.errors}
					/>
					<TextField
						labelText="Password"
						value={password.value}
						setValue={password.setValue}
						errors={password.errors}
						type="password"
					/>
					<CheckField
						labelText="Remember me"
						value={rememberMe}
						setValue={setRememberMe}
					/>
					<p className="app__helpful-text">
						<Link to="/users/forgot-password">I forgot my password</Link>
					</p>
				</Form>
			)}
			{isLoggedIn && (
				<p className="app__message-container">
					You are already logged in.
				</p>
			)}
		</>

	)
}

export default Login