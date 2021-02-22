import * as React from "react"

type InputErrorListTypes = {
	errors: string[],
	errorsWrapperClasses?: string,
	errorsClasses?: string,
	errorWrapperClasses?: string,
	errorClasses?: string
}

const FormErrorList: React.FunctionComponent<InputErrorListTypes> = ({
	errors,
	errorsWrapperClasses,
	errorsClasses,
	errorWrapperClasses,
	errorClasses
}: InputErrorListTypes): JSX.Element => {
	const hasErrors: boolean = errors && errors.length > 0

	const errorsWrapperComputedClasses = `form__errors-wrapper ${errorsWrapperClasses} ${hasErrors ? "form__errors-wrapper--has-errors" : ""}`

	const errorsComputedClasses = `form__errors ${errorsClasses} ${hasErrors ? "form__errors--has-errors" : ""}`

	const errorWrapperComputedClasses = `form__error-wrapper ${errorWrapperClasses}`

	const errorComputedClasses = `form__error ${errorClasses}`

	return (
		<section className={errorsWrapperComputedClasses}>
			<ul className={errorsComputedClasses}>
				{errors.map((error, i) => (
					<div
						key={i}
						className={errorWrapperComputedClasses}
					>
						<li className={errorComputedClasses}>
							{error}
						</li>
					</div>
				))}
			</ul>
		</section>
	)
}

export default FormErrorList