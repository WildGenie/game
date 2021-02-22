// Form input validator function
export interface FieldValidator<T> {
	(input: T): string
}

/**
 * Interface describing a form event handler
 *
 * The return value should indicate whether validation passes. Return true if validation should be considered passing, and false if it should be considered failing.
 */
export interface OnSubmitValidator {
	(): boolean | Promise<boolean>
}

export interface FormFieldEntry<T> {
	initialValue: T,
	validators?: FieldValidator<T>[]
}

export interface FormSelect {
	text: string | number,
	value: string | number
}

export interface RecaptchaWindow extends Window {
	grecaptcha: GRecaptcha
}

export interface GRecaptcha {
	reset(id: number): void
	ready(cb: VoidFunction): void
	render(elementId: string, options: RecaptchaRenderConfig): number
}

export interface RecaptchaRenderConfig {
	sitekey: string,
	callback: (response: string) => void
}