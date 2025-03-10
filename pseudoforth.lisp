(defun capture-word-decl (code)
  (let (result)
    (dolist (instruction code)
      (if (eq 'end instruction)
	  (return-from capture-word-decl
	    (reverse result))
	  (push instruction result)))))

(defun interpret-pseudoforth (code)
  (let (stack words variables)
    (loop with rest-code = code
	  for instruction = (pop rest-code)
	  while instruction
	  do
	     (case instruction
	       (print (format t "Print: ~a~%" (pop stack)))
	       (+ (push (+ (pop stack) (pop stack)) stack))
	       (- (push (- (pop stack) (pop stack)) stack))
	       (/ (push (/ (pop stack) (pop stack)) stack))
	       (* (push (* (pop stack) (pop stack)) stack))
	       (dup (push (first stack) stack))
	       (word (destructuring-bind (name &rest def)
			 (capture-word-decl rest-code)
		       (setf (getf words name) (reverse def))
		       (loop repeat (+ 2 (length def))
			     do (pop rest-code))))
	       (to (setf (getf variables (pop rest-code))
			 (pop stack)))
	       (t
		(if (symbolp instruction)
		    (let ((word (getf words
				      instruction))
			  (var (getf variables
				     instruction)))
		      (if word
			  (dolist (word-instr (getf words
						    instruction))
			    (push word-instr rest-code))
			  (push var stack)))
		    (push instruction stack)))))))

(defmacro pseudoforth (&body code)
  (interpret-pseudoforth code))
