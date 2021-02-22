import * as React from "react"

import { HintClassProps, InputStateProps } from "@/components/ui/forms/Types"

type HintProps = {
	text: string
} & HintClassProps & InputStateProps


const Hint: React.FunctionComponent<HintProps> = ({
	text,
	hintClasses = "",
	hintWrapperClasses = "",
	focused = false,
	hasErrors = false,
	required = false
}: HintProps): JSX.Element => {

	const hintWrapperComputedClasses = `form__hint-wrapper ${hintWrapperClasses} ${focused ? "form__hint-wrapper--focused" : ""} ${hasErrors ? "form__hint-wrapper--has-error": ""} ${required ? "form__hint-wrapper--required" : ""}`

	const hintComputedClasses = `form__hint ${hintClasses} ${focused ? "form__hint--focused" : ""} ${hasErrors ? "form__hint--has-error" : ""} ${required ? "form__hint--required" : ""}`

	return (
		<div className={hintWrapperComputedClasses}>
			<p className={hintComputedClasses}>
				{text}
			</p>
		</div>
	)
}

export default Hint